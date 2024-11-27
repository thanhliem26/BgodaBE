"use strict";

import BaseModel from "../helpers/baseModel";

module.exports = (sequelize, DataTypes) => {
  class Bank extends BaseModel {
   
    static associate(models) {
      // define association here
      Bank.hasMany(models.BusinessPartnerBank, {
        foreignKey: "BankId",
        as: "bank_data",
        onDelete: "cascade",
        hooks: true,
      });
    }

  }
  Bank.init(
    {
      Name: DataTypes.STRING,
      Code: DataTypes.STRING,
      Logo: DataTypes.STRING,
    },
    {
      sequelize,
      modelName: "Bank",
      tableName: "bank",
      timestamps: false, 
    }
  );
  return Bank;
};

