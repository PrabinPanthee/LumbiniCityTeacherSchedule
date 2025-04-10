CREATE PROCEDURE [dbo].[GenerateTimeSlotsForConfig]
    @ConfigId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete existing slots for this config to avoid duplicates
    DELETE FROM [dbo].[TimeSlot] WHERE [ConfigId] = @ConfigId;

    DECLARE @TotalClasses INT, @BreakAfterPeriod INT;
    DECLARE @StartTime TIME = '06:30', @CurrentTime TIME;
    DECLARE @Period INT = 1;

    -- Fetch config details
    SELECT 
        @TotalClasses = [TotalClasses],
        @BreakAfterPeriod = [BreakAfterPeriod]
    FROM [dbo].[SemesterScheduleConfig]
    WHERE [ConfigId] = @ConfigId;

    -- Validate config exists
    IF @TotalClasses IS NULL
    BEGIN
        RAISERROR('Invalid ConfigId: No configuration found.', 16, 1);
        RETURN;
    END

    SET @CurrentTime = @StartTime;

    -- Generate timeslots
    WHILE @Period <= @TotalClasses
    BEGIN
        -- Insert class slot
        INSERT INTO [dbo].[TimeSlot] (
            [ConfigId],
            [PeriodNumber],
            [StartTime],
            [EndTime],
            [Type]
        )
        VALUES (
            @ConfigId,
            @Period,
            @CurrentTime,
            DATEADD(MINUTE, 50, @CurrentTime),
            'class'
        );

        -- Move to next class/break time
        SET @CurrentTime = DATEADD(MINUTE, 50, @CurrentTime);

        -- Insert break after specified period
        IF @BreakAfterPeriod IS NOT NULL AND @Period = @BreakAfterPeriod
        BEGIN
            INSERT INTO [dbo].[TimeSlot] (
                [ConfigId],
                [PeriodNumber],
                [StartTime],
                [EndTime],
                [Type]
            )
            VALUES (
                @ConfigId,
                NULL, -- Breaks have no period number
                @CurrentTime,
                DATEADD(MINUTE, 30, @CurrentTime),
                'break'
            );

            SET @CurrentTime = DATEADD(MINUTE, 30, @CurrentTime);
        END

        SET @Period += 1;
    END
END

---Sumary---
--Algorithm for generating slots---
--Step 1: Declare the variables TotalClasses, BreakAfterPeriod, start time = starting time of uni,Current time (time pointer),
--    period (loop pointer)
--step 2: fetch the necessary data from the config table and if total classes are Null then return
--step 3: set current time as starttime 
--step 3: while period is <= totalclass then insert into slot table with config id, period number to track period,starttime is current time,
--        end time is current time  + 50 min tyepe is class 
---       now move the time for next class meaning now current time will be end time of previous class (current time = currenttime +50)
---       check if break exist so if exist then assign that in the same loop meaning period counter only counts period and if break is after 3rd then 
--        break will be after that period set current time for next class (current + 30)  then increment period check condition and continue or break 
--end 

-- time complexity for this algo is BigOH N +1 for data retrievaal which is one row  because loop runs until N so it is efficient and 