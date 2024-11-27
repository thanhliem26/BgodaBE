"use strict";

const BankService = require("../services/bank.service");
const {
  SuccessResponse,
} = require("../core/succes.response");

class BankController {

  getAllBanks = async (req, res, next) => {
    new SuccessResponse({
      message: "Get bank list success!",
      metadata: await BankService.getAllBank(req.query),
      options: {
        ...req.query,
      },
    }).send(res);
  };

}

module.exports = new BankController();
