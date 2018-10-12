import {Action} from "redux";

export interface IAction extends Action{
  type: any;
  payload: any;
}


