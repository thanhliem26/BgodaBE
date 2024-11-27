"use strict";

import BaseModel from "../helpers/baseModel";

module.exports = (sequelize, DataTypes) => {
  class BusinessPartnerBank extends BaseModel {
    static associate(models) {
      BusinessPartnerBank.belongsTo(models.Bank, {
        foreignKey: "BankId",
        as: "bank_data",
      });
    }
  }
  BusinessPartnerBank.init(
    {
      OwnerName: DataTypes.STRING,
      BankId: DataTypes.INTEGER,
      BusinessPartnerId: DataTypes.INTEGER,
      BankNumber: DataTypes.STRING,
      Status: DataTypes.ENUM("0", "1"),
    },
    {
      sequelize,
      modelName: "BusinessPartnerBank",
      tableName: "businessPartnerBank",
      timestamps: false, 
    }
  );
  return BusinessPartnerBank;
};
