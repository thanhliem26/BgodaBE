"use strict";

import BaseModel from "../helpers/baseModel";

module.exports = (sequelize, DataTypes) => {
  class emailAuthen extends BaseModel {
    static associate(models) {
      // define association here
      emailAuthen.belongsTo(models.account, {
        foreignKey: "AccountId",
        as: "account_data",
      });
    }
  }
  emailAuthen.init(
    {
      AccountId: DataTypes.STRING,
      Token: DataTypes.STRING,
      ExprireDate: DataTypes.STRING,
    },
    {
      sequelize,
      modelName: "emailAuthen",
      tableName: "emailAuthen",
      timestamps: false,
    }
  );
  return emailAuthen;
};
