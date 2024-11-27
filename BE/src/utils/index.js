"use strict";
const fs = require('fs');
const _ = require("lodash");
const { BadRequestError } = require('../core/error.response');

const getInfoData = ({ field = [], object = {} }) => {
  return _.pick(object, field);
};

const removeElement = ({ field = [], object = {} }) => {
  return _.omit(object, field);
};

const isJson = (str) => {
  try {
    JSON.parse(str);
  } catch (e) {
    return false;
  }
  return true;
};

const isNumber = (str) => {
  return !isNaN(str)
}

const deleteFIleUpload = (path) => {
  fs.unlink(path, (err) => {
    if (err) throw new BadRequestError('image not deleted!')
  });
}

const formatCurrency = (number) => {
  if (!number) return '';
  const strNumber = number.toString();
  const regex = /(\d)(?=(\d{3})+(?!\d))/g;
  return strNumber.replace(regex, "$1,");
}

module.exports = {
  getInfoData,
  removeElement,
  isJson,
  isNumber,
  deleteFIleUpload,
  formatCurrency
};
