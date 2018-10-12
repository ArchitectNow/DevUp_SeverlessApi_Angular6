// Define the global store shape by combining our application's
// reducers together into a given structure.
import {composeReducers, defaultFormReducer} from '@angular-redux/form';
import {combineReducers} from 'redux';
import {WodsReducer} from './wods.reducer';

export const rootReducer = composeReducers(
  defaultFormReducer(),
  combineReducers({
    wodsState: WodsReducer
  }));
