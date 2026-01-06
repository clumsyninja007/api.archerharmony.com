-- Add demo_url column to project table for demo site links
ALTER TABLE project ADD COLUMN demo_url VARCHAR(500) NULL AFTER live_url;
