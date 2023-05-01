SELECT 'CREATE DATABASE quests_sharp_auth' 
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'quests_sharp_auth')\gexec

SELECT 'CREATE DATABASE quests_sharp_generate' 
WHERE NOT EXISTS (SELECT FROM pg_database WHERE datname = 'quests_sharp_generate')\gexec