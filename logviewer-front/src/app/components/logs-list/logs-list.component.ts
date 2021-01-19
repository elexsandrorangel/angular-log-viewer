import {AfterViewInit, Component, OnInit, ViewChild} from '@angular/core';
import {LogsService} from '../../services/logs.service';
import {MatTableDataSource} from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-logs-list',
  templateUrl: './logs-list.component.html',
  styleUrls: ['./logs-list.component.scss']
})
export class LogsListComponent implements OnInit, AfterViewInit {

  logs: any[] = [];

  displayedColumnsMap: any[] = [
    {title: 'IP', field: 'remoteAddr'},
    {title: 'User', field: 'remoteUser'},
    {title: 'Date', field: 'timeLocal'},
    {title: 'URL', field: 'requestUrl'},
    {title: 'Status', field: 'statusCode'},
    {title: 'User Agent', field: 'httpUserAgent'},
    {title: 'action', field: ''}
  ];

  displayedColumnsNames: string[] = this.displayedColumnsMap.map(x => x.title);
  displayedColumnsWithValues: any[] = this.displayedColumnsMap.filter(x => x.field);

  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, {static: true}) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(private logsService: LogsService) {
  }

  ngOnInit(): void {
    this.renderLogs();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  sortData($event): void {
    const sortId = $event.active;
    const sortDir = $event.sortDirection;
    if ('asc' === sortDir){
      this.dataSource.data = this.logs.slice().sort(
        (a, b) => a[sortId] > b[sortId] ? -1 : a[sortId] < b[sortId] ? 1 : 0
      );
    } else {
      this.dataSource.data = this.logs.slice().sort(
        (a, b) => a[sortId] < b[sortId] ? -1 : a[sortId] > b[sortId] ? 1 : 0
      );
    }
  }

  renderLogs(): void {
    this.logsService.getAll().subscribe(l => {
       this.logs = l;
       this.dataSource.data = l;
     }, error => {
       console.log(error);
     });
  }

  deleteLog(log): void {
    this.logsService.delete(log.id).subscribe(res => {
      this.renderLogs();
    }, error => console.log(error));
  }

  openDialog(action: string, data: any): void {
    if (action.toLowerCase() === 'delete'){
      this.handleDeleteAlert(data);
    }
  }

  handleDeleteAlert(data: any): void {
    Swal.fire({
      title: 'Are you sure?',
      text: 'You will not be able to recover this log entry!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      cancelButtonText: 'No, keep it',
    }).then((result) => {
      if (result.isConfirmed) {
        this.deleteLog(data);
      }
    });
  }
}
