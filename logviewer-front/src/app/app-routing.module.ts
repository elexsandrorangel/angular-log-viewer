import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {LogsListComponent} from './components/logs-list/logs-list.component';
import {LogsImportComponent} from './components/logs-import/logs-import.component';
import {AddEditLogsComponent} from './components/add-edit-logs/add-edit-logs.component';

const routes: Routes = [
  {path: '', redirectTo: 'logs', pathMatch: 'full'},
  {path: 'logs', component: LogsListComponent},
  {path: 'logs/import', component: LogsImportComponent},
  {path: 'logs/add', component: AddEditLogsComponent},
  {path: 'logs/:id', component: AddEditLogsComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
