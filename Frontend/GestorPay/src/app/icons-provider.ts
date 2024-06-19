import { EnvironmentProviders, importProvidersFrom } from '@angular/core';
import {
  AppstoreOutline,
  BankOutline,
  DashboardOutline,
  DeleteOutline,
  FormOutline,
  IdcardOutline,
  MenuFoldOutline,
  MenuUnfoldOutline,
  PlusCircleFill,
  PlusCircleOutline,
  PlusCircleTwoTone,
  UserOutline
} from '@ant-design/icons-angular/icons';
import { NzIconModule } from 'ng-zorro-antd/icon';

const icons = [
  MenuFoldOutline,
  MenuUnfoldOutline,
  DashboardOutline,
  FormOutline,
  UserOutline,
  AppstoreOutline,
  BankOutline,
  IdcardOutline,
  DeleteOutline,
  PlusCircleOutline,
  PlusCircleTwoTone,
  PlusCircleFill
];

export function provideNzIcons(): EnvironmentProviders {
  return importProvidersFrom(NzIconModule.forRoot(icons));
}
