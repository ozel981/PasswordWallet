import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SharedPasswordWalletComponent } from './shared-password-wallet.component';

describe('SharedPasswordWalletComponent', () => {
  let component: SharedPasswordWalletComponent;
  let fixture: ComponentFixture<SharedPasswordWalletComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SharedPasswordWalletComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SharedPasswordWalletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
