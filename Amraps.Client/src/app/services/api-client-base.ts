import {catchError as _observableCatch, mergeMap as _observableMergeMap} from 'rxjs/operators';
import {Observable, of as _observableOf, throwError as _observableThrow} from 'rxjs';
import {Injectable, InjectionToken} from '@angular/core';
import {HttpClient, HttpHeaders, HttpResponse, HttpResponseBase} from '@angular/common/http';
import {SwaggerException} from './swagger-exception';
import {StandardExceptionVm} from './standard-exception-vm';
import {UnauthorizedExceptionVm} from './unauthorized-exception-vm';
import {WodsVm} from '../models/view.models';

export const ApiBaseUrl = new InjectionToken<string>('ApiBaseUrl');

@Injectable()
export abstract class ApiClientBase<TType>   {
  private http: HttpClient;
  private baseUrl: string;
  protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

 protected constructor(http: HttpClient, baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl ? baseUrl : 'http://localhost:4001/api';
  }

  public get(endPoint: string, processFunc: (s: string) => TType, id?: string): Observable<TType> {
    let url_ = this.baseUrl + endPoint;
    url_ = url_.replace(/[?&]$/, '');
    if (id && id.length) {
      url_ += `/${id}`;
    }

    const options_: any = {
      observe: 'response',
      responseType: 'blob',
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      })
    };

    return this.http.request('get', url_, options_).pipe(_observableMergeMap((response_: any) => {
      return this.processGet(response_, processFunc);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processGet(<any>response_, processFunc);
        } catch (e) {
          return <Observable<TType>><any>_observableThrow(e);
        }
      } else
        return <Observable<TType>><any>_observableThrow(response_);
    }));
  }

  protected processGet(response: HttpResponseBase, processFunc: (s: string) => TType): Observable<TType> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (<any>response).error instanceof Blob ? (<any>response).error : undefined;

    const _headers: any = {};
    if (response.headers) {
      for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); }
    }
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        const resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
        const returnResult = resultData200 ? processFunc(resultData200) : null;
        return _observableOf(returnResult);
      }));
    } else if (status === 400) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result400: any = null;
        const resultData400 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result400 = resultData400 ? StandardExceptionVm.fromJS(resultData400) : new StandardExceptionVm();
        return throwException('A server error occurred.', status, _responseText, _headers, result400);
      }));
    } else if (status === 401) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result401: any = null;
        const resultData401 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result401 = resultData401 ? UnauthorizedExceptionVm.fromJS(resultData401) : new UnauthorizedExceptionVm();
        return throwException('A server error occurred.', status, _responseText, _headers, result401);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException('An unexpected server error occurred.', status, _responseText, _headers);
      }));
    }
    return _observableOf<TType>(<TType>null);
  }

  public getMany(endPoint: string, processFunc: (s: string) => TType ): Observable<TType[]> {
    let url_ = this.baseUrl + endPoint;
    url_ = url_.replace(/[?&]$/, '');

    const options_: any = {
      observe: 'response',
      responseType: 'blob',
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      })
    };

    return this.http.request('get', url_, options_).pipe(_observableMergeMap((response_: any) => {
      return this.processGetMany(response_, processFunc);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processGetMany(<any>response_, processFunc);
        } catch (e) {
          return <Observable<TType[]>><any>_observableThrow(e);
        }
      } else
        return <Observable<TType[]>><any>_observableThrow(response_);
    }));
  }

  protected processGetMany(response: HttpResponseBase, processFunc: (s: string) => TType): Observable<TType[]> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (<any>response).error instanceof Blob ? (<any>response).error : undefined;

    const _headers: any = {};
    if (response.headers) {
      for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); }
    }
    if (status === 200) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result200: any = null;
        const resultData200 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
        if (resultData200 && resultData200.constructor === Array) {
          result200 = [];
          for (const item of resultData200)
            result200.push(processFunc(item));
        }
        return _observableOf(result200);
      }));
    } else if (status === 400) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result400: any = null;
        const resultData400 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result400 = resultData400 ? StandardExceptionVm.fromJS(resultData400) : new StandardExceptionVm();
        return throwException('A server error occurred.', status, _responseText, _headers, result400);
      }));
    } else if (status === 401) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result401: any = null;
        const resultData401 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result401 = resultData401 ? UnauthorizedExceptionVm.fromJS(resultData401) : new UnauthorizedExceptionVm();
        return throwException('A server error occurred.', status, _responseText, _headers, result401);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException('An unexpected server error occurred.', status, _responseText, _headers);
      }));
    }
    return _observableOf<TType[]>(<TType[]>null);
  }

  public delete(endPoint: string, id: string) {
    let url_ = this.baseUrl + endPoint;
    url_ = url_.replace(/[?&]$/, '');
    if (id && id.length)
      url_ += `/${id}`;

    const options_: any = {
      observe: 'response',
      responseType: 'blob',
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Accept': 'application/json'
      })
    };

    return this.http.request('delete', url_, options_).pipe(_observableMergeMap((response_: any) => {
      return this.processDelete(response_);
    })).pipe(_observableCatch((response_: any) => {
      if (response_ instanceof HttpResponseBase) {
        try {
          return this.processDelete(<any>response_);
        } catch (e) {
          return <Observable<boolean>><any>_observableThrow(e);
        }
      } else
        return <Observable<boolean>><any>_observableThrow(response_);
    }));
  }

  protected processDelete(response: HttpResponseBase): Observable<boolean> {
    const status = response.status;
    const responseBlob =
      response instanceof HttpResponse ? response.body :
        (<any>response).error instanceof Blob ? (<any>response).error : undefined;

    const _headers: any = {};
    if (response.headers) {
      for (const key of response.headers.keys()) { _headers[key] = response.headers.get(key); }
    }
    if (status === 201) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result201: any;
        const resultData201 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result201 = resultData201 !== undefined ? resultData201 : <any>null;
        return _observableOf(result201);
      }));
    } else if (status === 400) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result400: any = null;
        const resultData400 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result400 = resultData400 ? StandardExceptionVm.fromJS(resultData400) : new StandardExceptionVm();
        return throwException('A server error occurred.', status, _responseText, _headers, result400);
      }));
    } else if (status === 401) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        let result401: any = null;
        const resultData401 = _responseText === '' ? null : JSON.parse(_responseText, this.jsonParseReviver);
        result401 = resultData401 ? UnauthorizedExceptionVm.fromJS(resultData401) : new UnauthorizedExceptionVm();
        return throwException('A server error occurred.', status, _responseText, _headers, result401);
      }));
    } else if (status !== 200 && status !== 204) {
      return blobToText(responseBlob).pipe(_observableMergeMap(_responseText => {
        return throwException('An unexpected server error occurred.', status, _responseText, _headers);
      }));
    }
    return _observableOf<boolean>(<any>null);
  }
}

function throwException(message: string, status: number, response: string,
                        headers: { [key: string]: any; }, result?: any): Observable<any> {
  if (result !== null && result !== undefined)
    return _observableThrow(result);
  else
    return _observableThrow(new SwaggerException(message, status, response, headers, null));
}

function blobToText(blob: any): Observable<string> {
  return new Observable<string>((observer: any) => {
    if (!blob) {
      observer.next('');
      observer.complete();
    } else {
      const reader = new FileReader();
      reader.onload = function() {
        observer.next(this.result);
        observer.complete();
      };
      reader.readAsText(blob);
    }
  });
}



