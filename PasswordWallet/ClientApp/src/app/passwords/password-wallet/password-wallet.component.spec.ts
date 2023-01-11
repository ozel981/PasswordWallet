import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PasswordWalletComponent } from './password-wallet.component';

describe('PasswordWalletComponent', () => {
  let component: PasswordWalletComponent;
  let fixture: ComponentFixture<PasswordWalletComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PasswordWalletComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PasswordWalletComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
