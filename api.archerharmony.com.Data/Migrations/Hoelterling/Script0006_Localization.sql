-- Create language reference table
CREATE TABLE IF NOT EXISTS language (
    code VARCHAR(10) PRIMARY KEY,
    name VARCHAR(50) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Insert supported languages and locales
INSERT INTO language (code, name) VALUES
('en', 'English'),
('en-US', 'English (United States)'),
('en-GB', 'English (United Kingdom)'),
('de', 'Deutsch'),
('de-DE', 'Deutsch (Deutschland)'),
('de-AT', 'Deutsch (Österreich)'),
('de-CH', 'Deutsch (Schweiz)');

-- Localized person data
CREATE TABLE IF NOT EXISTS person_localized (
    id INT AUTO_INCREMENT PRIMARY KEY,
    person_id INT NOT NULL,
    language_code VARCHAR(10) NOT NULL,
    title VARCHAR(255) NOT NULL,
    hero_description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY unique_person_language (person_id, language_code),
    CONSTRAINT fk_person_localized_person
        FOREIGN KEY (person_id)
        REFERENCES person(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_person_localized_language
        FOREIGN KEY (language_code)
        REFERENCES language(code)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Localized contact data
CREATE TABLE IF NOT EXISTS contact_localized (
    id INT AUTO_INCREMENT PRIMARY KEY,
    contact_id INT NOT NULL,
    language_code VARCHAR(10) NOT NULL,
    label VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY unique_contact_language (contact_id, language_code),
    CONSTRAINT fk_contact_localized_contact
        FOREIGN KEY (contact_id)
        REFERENCES contact(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_contact_localized_language
        FOREIGN KEY (language_code)
        REFERENCES language(code)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Localized work experience data
CREATE TABLE IF NOT EXISTS work_experience_localized (
    id INT AUTO_INCREMENT PRIMARY KEY,
    work_experience_id INT NOT NULL,
    language_code VARCHAR(10) NOT NULL,
    title VARCHAR(255) NOT NULL,
    company VARCHAR(255) NOT NULL,
    location VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY unique_work_experience_language (work_experience_id, language_code),
    CONSTRAINT fk_work_experience_localized_work_experience
        FOREIGN KEY (work_experience_id)
        REFERENCES work_experience(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_work_experience_localized_language
        FOREIGN KEY (language_code)
        REFERENCES language(code)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Localized work experience skills data
CREATE TABLE IF NOT EXISTS work_experience_skills_localized (
    id INT AUTO_INCREMENT PRIMARY KEY,
    work_experience_skill_id INT NOT NULL,
    language_code VARCHAR(10) NOT NULL,
    skill VARCHAR(500) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY unique_work_experience_skill_language (work_experience_skill_id, language_code),
    CONSTRAINT fk_work_experience_skills_localized_work_experience_skills
        FOREIGN KEY (work_experience_skill_id)
        REFERENCES work_experience_skills(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_work_experience_skills_localized_language
        FOREIGN KEY (language_code)
        REFERENCES language(code)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Localized skills data
CREATE TABLE IF NOT EXISTS skills_localized (
    id INT AUTO_INCREMENT PRIMARY KEY,
    skill_id INT NOT NULL,
    language_code VARCHAR(10) NOT NULL,
    label VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY unique_skill_language (skill_id, language_code),
    CONSTRAINT fk_skills_localized_skills
        FOREIGN KEY (skill_id)
        REFERENCES skills(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_skills_localized_language
        FOREIGN KEY (language_code)
        REFERENCES language(code)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Localized education data
CREATE TABLE IF NOT EXISTS education_localized (
    id INT AUTO_INCREMENT PRIMARY KEY,
    education_id INT NOT NULL,
    language_code VARCHAR(10) NOT NULL,
    school VARCHAR(255) NOT NULL,
    degree_earned VARCHAR(255),
    major VARCHAR(255),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY unique_education_language (education_id, language_code),
    CONSTRAINT fk_education_localized_education
        FOREIGN KEY (education_id)
        REFERENCES education(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_education_localized_language
        FOREIGN KEY (language_code)
        REFERENCES language(code)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;
