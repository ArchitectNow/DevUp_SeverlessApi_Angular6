import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditWodComponent } from './edit-wod.component';

describe('EditWodComponent', () => {
  let component: EditWodComponent;
  let fixture: ComponentFixture<EditWodComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditWodComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditWodComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
