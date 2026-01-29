-- Remove company column from work_experience_localized since company names are proper nouns and shouldn't be translated
ALTER TABLE work_experience_localized DROP COLUMN IF EXISTS company;