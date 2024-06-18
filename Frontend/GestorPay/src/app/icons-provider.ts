import { EnvironmentProviders, importProvidersFrom } from '@angular/core';
import {
  DashboardOutline,
  DeleteOutline,
  EyeInvisibleOutline,
  EyeOutline,
  FileAddOutline,
  FormOutline,
  GroupOutline,
  HistoryOutline,
  IdcardOutline,
  LoadingOutline,
  LockOutline,
  MenuFoldOutline,
  MenuUnfoldOutline,
  TeamOutline,
  UserAddOutline,
  UserOutline,
} from '@ant-design/icons-angular/icons';
import { NzIconModule } from 'ng-zorro-antd/icon';

const icons = [
  MenuFoldOutline,
  MenuUnfoldOutline,
  DashboardOutline,
  FormOutline,
  LockOutline,
  IdcardOutline,
  DeleteOutline,
  FileAddOutline,
  GroupOutline,
  TeamOutline,
  EyeInvisibleOutline,
  EyeOutline,
  LoadingOutline,
  HistoryOutline,
  UserOutline,
  UserAddOutline,
];

export function provideNzIcons(): EnvironmentProviders {
  return importProvidersFrom(NzIconModule.forRoot(icons));
}
