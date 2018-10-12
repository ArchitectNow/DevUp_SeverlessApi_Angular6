import {Reducer} from 'redux';
import {WodsState} from '../wods.state';
import {IAction} from '../action.model';

export const WodsReducer: Reducer<WodsState> = (state: WodsState = new WodsState(), action: IAction): WodsState => {
  const payload = <WodsState>action.payload;
  switch (action.type) {
    case '': {
      const newState = Object.assign({}, state, payload);
      return newState;
    }
    default:
      return state;
  }
}
