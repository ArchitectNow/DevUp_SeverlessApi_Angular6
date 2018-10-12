import * as moment from 'moment';

export interface IViewModel {
  init(data?: any);
}

export interface IMeasureVm {
  name: string;
  value: any;
}

export class MeasureVm implements IMeasureVm {

  constructor(data?: IMeasureVm) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  name: string;
  value: any;

  static fromJS(data: any): MeasureVm {
    data = typeof data === 'object' ? data : {};
    const result = new MeasureVm();
    result.init(data);
    return result;
  }

  init(data?: any) {
    if (data) {
      this.name = data['name'] !== undefined ? data['name'] : <any>null;
      this.value = data['value'] !== undefined ? data['value'] : <any>null;
    }
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data['name'] = this.name !== undefined ? this.name : <any>null;
    data['value'] = this.value !== undefined ? this.value : <any>null;
    return data;
  }
}

export interface ISectionVm {
  prefix: string;
  displayName: string;
  componentId: string;
  repScheme: string;
  details: string;
  measureType: number;
  measure: MeasureVm[];
}

export class SectionVm implements ISectionVm {

  constructor(data?: ISectionVm) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  componentId: string;
  details: string;
  displayName: string;
  measure: MeasureVm[];
  measureType: number;
  prefix: string;
  repScheme: string;

  static fromJS(data: any): SectionVm {
    data = typeof data === 'object' ? data : {};
    const result = new SectionVm();
    result.init(data);
    return result;
  }

  init(data?: any) {
    if (data) {
      this.componentId = data['componentId'] !== undefined ? data['componentId'] : <any>null;
      this.details = data['details'] !== undefined ? data['details'] : <any>null;
      this.displayName = data['displayName'] !== undefined ? data['displayName'] : <any>null;
      if (data['measure'] && data['measure'].constructor === Array) {
        this.measure = [];
        for (const item of data['measure'])
          this.measure.push(MeasureVm.fromJS(item));
      }
      this.measureType = data['measureType'] !== undefined ? data['measureType'] : <any>null;
      this.prefix = data['prefix'] !== undefined ? data['prefix'] : <any>null;
      this.repScheme = data['repScheme'] !== undefined ? data['repScheme'] : <any>null;
    }
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data['componentId'] = this.componentId !== undefined ? this.componentId : <any>null;
    data['details'] = this.details !== undefined ? this.details : <any>null;
    data['displayName'] = this.displayName !== undefined ? this.displayName : <any>null;
    if (this.measure && this.measure.constructor === Array) {
      data['measure'] = [];
      for (const item of this.measure)
        data['measure'].push(item.toJSON());
    }
    data['measureType'] = this.measureType !== undefined ? this.measureType : <any>null;
    data['prefix'] = this.prefix !== undefined ? this.prefix : <any>null;
    data['repScheme'] = this.repScheme !== undefined ? this.repScheme : <any>null;
    return data;
  }

}

export interface IWodsVm {
  id: string;
  locationId: string;
  programId: string;
  program: string;
  name: string;
  wodDate: moment.Moment;
  publishOnDateTime: moment.Moment;
  sections: SectionVm[];
  notes: string;
}

export class WodsVm implements IWodsVm {

  constructor(data?: ISectionVm) {
    if (data) {
      for (const property in data) {
        if (data.hasOwnProperty(property))
          (<any>this)[property] = (<any>data)[property];
      }
    }
  }
  id: string;
  locationId: string;
  name: string;
  notes: string;
  program: string;
  programId: string;
  publishOnDateTime: moment.Moment;
  sections: SectionVm[];
  wodDate: moment.Moment;

  static fromJS(data: any): WodsVm {
    data = typeof data === 'object' ? data : {};
    const result = new WodsVm();
    result.init(data);
    return result;
  }

  init(data?: any) {
    if (data) {
      this.id = data['id'] !== undefined ? data['id'] : <any>null;
      this.locationId = data['locationId'] !== undefined ? data['locationId'] : <any>null;
      this.name = data['name'] !== undefined ? data['name'] : <any>null;
      this.notes = data['notes'] !== undefined ? data['notes'] : <any>null;
      this.program = data['program'] !== undefined ? data['program'] : <any>null;
      this.programId = data['programId'] !== undefined ? data['programId'] : <any>null;
      this.publishOnDateTime = data['publishOnDateTime'] ? moment(data['publishOnDateTime'].toString()) : <any>null;
      this.wodDate = data['wodDate'] ? moment(data['wodDate'].toString()) : <any>null;

      if (data['sections'] && data['sections'].constructor === Array) {
        this.sections = [];
        for (const item of data['sections'])
          this.sections.push(SectionVm.fromJS(item));
      }
    }
  }

  toJSON(data?: any) {
    data = typeof data === 'object' ? data : {};
    data['id'] = this.id !== undefined ? this.id : <any>null;
    data['locationId'] = this.locationId !== undefined ? this.locationId : <any>null;
    data['name'] = this.name !== undefined ? this.name : <any>null;
    if (this.sections && this.sections.constructor === Array) {
      data['sections'] = [];
      for (const item of this.sections)
        data['sections'].push(item.toJSON());
    }
    data['notes'] = this.notes !== undefined ? this.notes : <any>null;
    data['program'] = this.program !== undefined ? this.program : <any>null;
    data['programId'] = this.programId !== undefined ? this.programId : <any>null;
    data['publishOnDateTime'] = this.publishOnDateTime ? this.publishOnDateTime.toISOString() : <any>null;
    data['wodDate'] =  this.wodDate ? this.wodDate.toISOString() : <any>null;
    return data;
  }
}
