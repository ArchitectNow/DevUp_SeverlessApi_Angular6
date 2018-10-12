
export interface IValidationErrorVm {
  target: any;
  property: string;
  value: string;
  constraints: any;
}

export class ValidationErrorVm implements IValidationErrorVm {

  constructor(data?: IValidationErrorVm) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }

  target: any;
  property: string;
  value: string;
  constraints: any;

  static fromJS(data: any): ValidationErrorVm {
    data = typeof data === 'object' ? data : {};
    const result = new ValidationErrorVm();
    result.init(data);
    return result;
  }

  init(data?: any) {
    if (data) {
      this.target = data['target'] !== undefined ? data['target'] : <any>null;
      this.property = data['property'] !== undefined ? data['property'] : <any>null;
      this.value = data['value'] !== undefined ? data['value'] : <any>null;
      this.constraints = data['constraints'] !== undefined ? data['constraints'] : <any>null;
    }
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data['target'] = this.target !== undefined ? this.target : <any>null;
    data['property'] = this.property !== undefined ? this.property : <any>null;
    data['value'] = this.value !== undefined ? this.value : <any>null;
    data['constraints'] = this.constraints !== undefined ? this.constraints : <any>null;
    return data;
  }
}
