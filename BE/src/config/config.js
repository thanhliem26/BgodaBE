require('dotenv').config();

module.exports = {
  "development": {
    "username": process.env.DEV_DB_USERNAME,
    "password": process.env.DEV_DB_PWD,
    "database": process.env.DEV_DB_NAME,
    "host": process.env.DEV_DB_HOST,
    "port": process.env.DEV_DB_PORT,
    "dialect": process.env.DEV_DB_DIALECT,
  },
  "test": {
    "username": "root",
    "password": null,
    "database": "database_test",
    "host": "127.0.0.1",
    "dialect": "mysql"
  },
  "production": {
    "username": process.env.PRO_DB_USERNAME,
    "password": process.env.PRO_DB_PWD,
    "database": process.env.PRO_DB_NAME,
    "host": process.env.PRO_DB_HOST,
    "port": process.env.PRO_DB_PORT,
    "dialect": process.env.PRO_DB_DIALECT,
  }
}
