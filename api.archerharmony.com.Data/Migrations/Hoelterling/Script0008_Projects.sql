-- Create projects table
CREATE TABLE IF NOT EXISTS project (
    id INT AUTO_INCREMENT PRIMARY KEY,
    person_id INT NOT NULL,
    title VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    long_description TEXT NOT NULL,
    image_url VARCHAR(500),
    live_url VARCHAR(500),
    github_url VARCHAR(500),
    display_order INT NOT NULL DEFAULT 0,
    is_active BOOLEAN NOT NULL DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT fk_project_person
        FOREIGN KEY (person_id)
        REFERENCES person(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    INDEX idx_person_display_order (person_id, display_order)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create project technologies table (many-to-many relationship)
CREATE TABLE IF NOT EXISTS project_technology (
    id INT AUTO_INCREMENT PRIMARY KEY,
    project_id INT NOT NULL,
    technology VARCHAR(100) NOT NULL,
    display_order INT NOT NULL DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    CONSTRAINT fk_project_technology_project
        FOREIGN KEY (project_id)
        REFERENCES project(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    INDEX idx_project_display_order (project_id, display_order)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create localized project data table
CREATE TABLE IF NOT EXISTS project_localized (
    id INT AUTO_INCREMENT PRIMARY KEY,
    project_id INT NOT NULL,
    language_code VARCHAR(10) NOT NULL,
    title VARCHAR(255) NOT NULL,
    description TEXT NOT NULL,
    long_description TEXT NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY unique_project_language (project_id, language_code),
    CONSTRAINT fk_project_localized_project
        FOREIGN KEY (project_id)
        REFERENCES project(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_project_localized_language
        FOREIGN KEY (language_code)
        REFERENCES language(code)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

-- Create localized project technology table
CREATE TABLE IF NOT EXISTS project_technology_localized (
    id INT AUTO_INCREMENT PRIMARY KEY,
    project_technology_id INT NOT NULL,
    language_code VARCHAR(10) NOT NULL,
    technology VARCHAR(100) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    UNIQUE KEY unique_project_technology_language (project_technology_id, language_code),
    CONSTRAINT fk_project_technology_localized_project_technology
        FOREIGN KEY (project_technology_id)
        REFERENCES project_technology(id)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
    CONSTRAINT fk_project_technology_localized_language
        FOREIGN KEY (language_code)
        REFERENCES language(code)
        ON DELETE CASCADE
        ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;