﻿
-- START CHANGE SCRIPT v1.0/001_change.sql (1)

INSERT INTO ChangeLog (ChangeId, Folder, ScriptNumber, ScriptName, StartDate, AppliedBy, ScriptStatus, ScriptOutput)
 VALUES (SYS_GUID(), 'v1.0', 1, '001_change.sql', CURRENT_TIMESTAMP, USER, 3, ' ');

-- contents of change script 1

UPDATE ChangeLog 
SET CompleteDate = SYSDATE, ScriptStatus = 1, ScriptOutput = ' '
WHERE Folder = 'v1.0' AND ScriptNumber = 1;

COMMIT;

-- END CHANGE SCRIPT v1.0/001_change.sql (1)


-- START CHANGE SCRIPT v1.0/002_change.sql (2)

INSERT INTO ChangeLog (ChangeId, Folder, ScriptNumber, ScriptName, StartDate, AppliedBy, ScriptStatus, ScriptOutput)
 VALUES (SYS_GUID(), 'v1.0', 2, '002_change.sql', CURRENT_TIMESTAMP, USER, 3, ' ');

-- contents of change script 2

UPDATE ChangeLog 
SET CompleteDate = SYSDATE, ScriptStatus = 1, ScriptOutput = ' '
WHERE Folder = 'v1.0' AND ScriptNumber = 2;

COMMIT;

-- END CHANGE SCRIPT v1.0/002_change.sql (2)

