'use strict'

const express = require('express');
const AccessController = require('../../controllers/access.controller');
const router = express.Router();
const  asyncHandler = require('../../helpers/asyncHandler');

import multer from 'multer';
const upload = multer({ dest: 'uploads/' });

router.post('/uploadFIleS3', upload.single('file'), asyncHandler(AccessController.uploadFileS3)); 

module.exports = router;