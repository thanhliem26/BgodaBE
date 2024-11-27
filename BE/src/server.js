const app = require("./app");
require("dotenv").config();

const PORT = process.env.PORT || 8000;

const server = app.listen(PORT, () => {
  console.log(`Project is running with port ${PORT}`);
});

process.on("message", (message) => {
  console.log('check', message);
});

process.on("SIGINT", () => {
  server.close(() => {
    console.log("Exit Server Express");
  });
});
