("use strict");
import db from "../models";

class BankService {

  static getAllBank = async (query) => {
    const page = +query.page || 1;
    const limit = +query.limit || 10;

    return await db.Bank.findAndCountAll({
      limit: limit,
      offset: (page - 1) * limit,
    });
  };
}

module.exports = BankService;
