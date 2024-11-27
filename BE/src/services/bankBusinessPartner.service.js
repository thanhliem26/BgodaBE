("use strict");
import { where } from "sequelize";
import db from "../models";

class BankBusinessPartnerService {

  static getAllBankBusinessPartner = async (query) => {
    const page = +query.page || 1;
    const limit = +query.limit || 10;

    return await db.BusinessPartnerBank.findAndCountAll({
      limit: limit,
      offset: (page - 1) * limit,
    });
  };

  static getBankByBusinessPartner = async (query) => {
    const page = +query.page || 1;
    const limit = +query.limit || 10;

    const businessPartnerId = query.businessPartnerId;

    return await db.BusinessPartnerBank.findAndCountAll({
      limit: limit,
      offset: (page - 1) * limit,
      where: {businessPartnerId},
      include: [{ model: db.Bank, as: "bank_data" }],
    });
  };
}

module.exports = BankBusinessPartnerService;
