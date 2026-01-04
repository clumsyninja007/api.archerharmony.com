-- Add display_order column to work_experience_skills to support reordering
ALTER TABLE work_experience_skills ADD COLUMN IF NOT EXISTS display_order INT DEFAULT 0;

-- Create index for performance when ordering
CREATE INDEX IF NOT EXISTS idx_work_experience_skills_display_order ON work_experience_skills(work_experience_id, display_order);