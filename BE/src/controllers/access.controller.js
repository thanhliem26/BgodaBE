'use strict'

const AccessService = require("../services/access.service");
const { OK, CREATED, SuccessResponse, UPDATED } = require('../core/succes.response');
class AccessController {
    uploadFileS3 = async (req, res, next) => {
        new CREATED({
            message: 'upload file success!',
            metadata: await AccessService.uploadFileServiceS3(req.file, req.body),
        }).send(res)
    }
}

module.exports = new AccessController