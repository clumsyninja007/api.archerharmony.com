-- Add audit tracking columns to base tables
ALTER TABLE person ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE work_experience ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE work_experience_skills ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE education ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE skills ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE project ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE project_technology ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE contact ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;

-- Add audit tracking columns to localized tables
ALTER TABLE person_localized ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE work_experience_localized ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE work_experience_skills_localized ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE education_localized ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE skills_localized ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE project_localized ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE project_technology_localized ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;
ALTER TABLE contact_localized ADD COLUMN IF NOT EXISTS updated_by VARCHAR(255) NULL;

-- Add indexes for foreign key lookups (performance optimization)
CREATE INDEX IF NOT EXISTS idx_work_experience_person_id ON work_experience(person_id);
CREATE INDEX IF NOT EXISTS idx_education_person_id ON education(person_id);
CREATE INDEX IF NOT EXISTS idx_skills_person_id ON skills(person_id);
CREATE INDEX IF NOT EXISTS idx_contact_person_id ON contact(person_id);