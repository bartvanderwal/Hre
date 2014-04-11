-- Bart van der Wal - 27-07-2013:
-- Add column 'AttachmentFilePath' to the table 'Newsletter' to allow adding a file attached to the newsletter e-mails.

-- Undo queries.
-- alter table Newsletter drop AttachmentFilePath;
-- End of undo queries.

-- Add a column for AttachmentFilePath.
alter table newsletter add AttachmentFilePath varchar(127);

