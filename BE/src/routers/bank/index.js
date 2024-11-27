'use strict'

const express = require('express');
const BankController = require('../../controllers/bank.controller');
const asyncHandler = require('../../helpers/asyncHandler');
const router = express.Router();

router.get('/list', asyncHandler(BankController.getAllBanks));


module.exports = router;