const { Model } = require('sequelize');
import { isEmpty } from 'lodash';
import { BadRequestError, NotFoundError } from '../core/error.response';
import lodash from 'lodash';
class BaseModel extends Model {
  static async findOneAndUpdate({filter = {}, values = {}, options = {} }) {
    try {
    const instance = !lodash.isEmpty(filter) ? await this.findOne({where: {...filter}}) : null;
    if(!instance && isEmpty(options) && !options?.upsert) return null;

    const data = {...filter, ...values};
    delete data.id;

    if(!instance && options?.upsert) {
      const newInstance = await this.create({...data});
      return newInstance;
    }

    await instance.update({...values});
    return instance;
    } catch(err) {
      console.log("error", err)
      throw new BadRequestError(err.message)
    }
  }
}

module.exports = BaseModel;