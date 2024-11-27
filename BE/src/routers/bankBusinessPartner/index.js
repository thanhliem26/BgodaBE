'use strict'

const express = require('express');
const BankBusinessPartnerController = require('../../controllers/bankBusinessPartner.controller');
const asyncHandler = require('../../helpers/asyncHandler');
const router = express.Router();

router.get('/list', asyncHandler(BankBusinessPartnerController.getAllBanksBusinessPartner));
router.get('/bank', asyncHandler(BankBusinessPartnerController.getBankBusinessPartner));


module.exports = router;