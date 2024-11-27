"use strict";

const BankBusinessPartnerService = require("../services/bankBusinessPartner.service");
const {
  SuccessResponse,
} = require("../core/succes.response");

class BankController {

  getAllBanksBusinessPartner = async (req, res, next) => {
    new SuccessResponse({
      message: "Get bank list success!",
      metadata: await BankBusinessPartnerService.getAllBankBusinessPartner(req.query),
      options: {
        ...req.query,
      },
    }).send(res);
  };

  getBankBusinessPartner = async (req, res, next) => {
    new SuccessResponse({
      message: "Get bank list success!",
      metadata: await BankBusinessPartnerService.getBankByBusinessPartner(req.query),
      options: {
        ...req.query,
      },
    }).send(res);
  };

}

module.exports = new BankController();
