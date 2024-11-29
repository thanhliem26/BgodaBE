"use strict";
const {
  BadRequestError,
} = require("../core/error.response");
import { deleteFIleUpload } from "../utils";
import { uploadFileS3 } from "../utils/aws";
const fs = require("fs").promises;
import db from "../models";
class AccessService {
  static uploadFileServiceS3 = async (file, data) => {
    if (!file || !data.nameFile) {
      throw new BadRequestError("File and nameFile is required!");
    }

    const pathImage = file.path;
    const nameImage = data.nameFile;

    try {
      const fileData = await fs.readFile(pathImage);
      const fileS3 = await uploadFileS3(fileData, nameImage);

      //delete file in folder upload
      deleteFIleUpload(pathImage);

      return fileS3;
    } catch (err) {
      throw err;
    }
  };

  static activeEmail = async (data) => {
    try {
      const emailAuthen = await db.emailAuthen.findOne({
        where: {Token: data?.token},
        // include: [{ model: db.account, as: "account_data" }],
      });
      if(!emailAuthen) return BadRequestError('active failed')
     
      const account = await db.account.findOne({
        where: {id: emailAuthen?.AccountId},
        // include: [{ model: db.account, as: "account_data" }],
      });
      account.EmailActive = 1;
      await account.save();
      await db.emailAuthen.destroy({
        where: {Token: data?.token},
      });
  
      return 'active success'
    } catch (err) {
      throw err;
    }
  };
}

module.exports = AccessService;
