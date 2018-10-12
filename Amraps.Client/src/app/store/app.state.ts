import {WodsState} from './wods.state';

export interface IAppState {
  wodsState: WodsState;
}

export const AppState: IAppState = {
  wodsState: new WodsState()
}
