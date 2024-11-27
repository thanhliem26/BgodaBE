// const express = require('express');
import express from "express";
import { connection } from "./config/connectDB";
import cors from "cors";
const morgan = require("morgan");
const helmet = require("helmet");
const compression = require("compression");
require("dotenv").config();
const bodyParser = require('body-parser');

const app = express();

//encode data when use method post
app.use(express.json({limit: '50mb'}));
app.use(bodyParser.json({ limit: '50mb' }));
app.use(express.urlencoded({ extended: true }));

//init middleware
app.use(morgan("dev"));
app.use(helmet()); //hidden framework using
app.use(compression());

const corsOptions = {
  origin: process.env.URL_FE_CORS,
  methods: "GET,HEAD,PUT,PATCH,POST,DELETE",
  optionsSuccessStatus: 204,
};

app.use(cors(corsOptions));

//init db
connection();

//init route
app.use("/", require("./routers"));

//handle errors
app.use((req, res, next) => {
  const error = new Error("Not Found");
  error.status = 404;
  next(error);
});

app.use((error, req, res, next) => {
  const statusCode = error.status || 500;
  console.log("🚀 ~ statusCode:", statusCode)

  return res.status(statusCode).json({
    status: "error",
    code: statusCode,
    message: error.message || "Internal Server Error",
  });
});
//handle errors

module.exports = app;
