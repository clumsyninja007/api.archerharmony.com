-- Add hero_description column to person table for English default
ALTER TABLE person
ADD COLUMN hero_description TEXT NULL AFTER title;