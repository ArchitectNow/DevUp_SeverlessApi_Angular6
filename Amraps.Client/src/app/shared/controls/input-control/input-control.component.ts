import {AfterViewInit, Component, ElementRef, forwardRef, Input, OnChanges, OnInit, SimpleChanges, ViewChild} from '@angular/core';
import {ControlValueAccessor, FormControl, NG_VALIDATORS, NG_VALUE_ACCESSOR} from '@angular/forms';

let _uniqueInputId = 1;

@Component({
  selector: 'input-control',
  templateUrl: './input-control.component.html',
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => InputControlComponent),
    multi: true
  }]
})
export class InputControlComponent implements AfterViewInit, ControlValueAccessor {
  @Input() public controlLabel = false;
  @Input() public inputType = 'text';
  @Input() public disabled = false;
  @Input() public maxLength: number = null;
  @Input() public minLength: number = null;
  @Input() public formControl: FormControl;
  @ViewChild('#control') inputRef: ElementRef;

  public id = `input-control-${_uniqueInputId++}`;
  public value: any;

  constructor() {
  }

  ngAfterViewInit(): void {
    this.value = this.formControl.value;
    this.inputRef.nativeElement.maxLength = this.maxLength;
  }

  private propagateChange = (v: any) => {};
  private propagateTouch = () => {};

  public registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }

  public registerOnTouched(fn: any): void {
    this.propagateTouch = fn;
  }

  public setDisabledState(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  public writeValue(obj: any): void {
    this.value = obj;
    this.propagateChange(obj);
  }

  public focusInput() {
    this.inputRef.nativeElement.focus();
    this.propagateTouch();
  }

  public changed(evt: Event, value: any) {
    this.value = value;
    this.writeValue(this.value);
  }


}
