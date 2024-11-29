"use strict";

import BaseModel from "../helpers/baseModel";

module.exports = (sequelize, DataTypes) => {
  class Account extends BaseModel {
    static associate(models) {
      // define association here
      Account.hasOne(models.emailAuthen, {
          foreignKey: "AccountId",
          as: "account_data",
          onDelete: "cascade",
          hooks: true,
        });
    }
  }
  Account.init(
    {
      Email: DataTypes.STRING,
      Password: DataTypes.STRING,
      UserType: DataTypes.STRING,
      Salt: DataTypes.STRING,
      EmailActive: DataTypes.INTEGER,
      RoleId: DataTypes.INTEGER
    },
    {
      sequelize,
      modelName: "account",
      tableName: "account",
      timestamps: false,
    }
  );
  return Account;
};
