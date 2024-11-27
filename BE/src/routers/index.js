'use strict'

const express = require('express');
const router = express.Router();

//router handle
router.use('/v1/api/access', require('./access'));
router.use('/v1/api/bank', require('./bank'));
router.use('/v1/api/bank-business-partner', require('./bankBusinessPartner'));

router.get('/', (req, res, next) => {
    return res.status(200).json({
        message: 'Hello word',
    })
})


module.exports = router;