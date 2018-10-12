import {StandardExceptionVmCode} from './standard-exception-vm-code';
import {ValidationErrorVm} from './validation-error-vm';


export interface IStandardExceptionVm {
  message: string;
  statusCode: number;
  error: string;
  /** Array of validation errors */
  validationErrors?: ValidationErrorVm[] | null;
  code: StandardExceptionVmCode;
}

export class StandardExceptionVm implements IStandardExceptionVm {

  constructor(data?: IStandardExceptionVm) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  message: string;
  statusCode: number;
  error: string;
  /** Array of validation errors */
  validationErrors?: ValidationErrorVm[] | null;
  code: StandardExceptionVmCode;

  static fromJS(data: any): StandardExceptionVm {
    data = typeof data === 'object' ? data : {};
    const result = new StandardExceptionVm();
    result.init(data);
    return result;
  }

  init(data?: any) {
    if (data) {
      this.message = data['message'] !== undefined ? data['message'] : <any>null;
      this.statusCode = data['statusCode'] !== undefined ? data['statusCode'] : <any>null;
      this.error = data['error'] !== undefined ? data['error'] : <any>null;
      if (data['validationErrors'] && data['validationErrors'].constructor === Array) {
        this.validationErrors = [];
        for (const item of data['validationErrors'])
          this.validationErrors.push(ValidationErrorVm.fromJS(item));
      }
      this.code = data['code'] !== undefined ? data['code'] : <any>null;
    }
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data['message'] = this.message !== undefined ? this.message : <any>null;
    data['statusCode'] = this.statusCode !== undefined ? this.statusCode : <any>null;
    data['error'] = this.error !== undefined ? this.error : <any>null;
    if (this.validationErrors && this.validationErrors.constructor === Array) {
      data['validationErrors'] = [];
      for (const item of this.validationErrors)
        data['validationErrors'].push(item.toJSON());
    }
    data['code'] = this.code !== undefined ? this.code : <any>null;
    return data;
  }
}
