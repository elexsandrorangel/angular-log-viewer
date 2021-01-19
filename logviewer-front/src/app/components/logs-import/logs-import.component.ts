import {Component, OnInit} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {LogsService} from '../../services/logs.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-logs-import',
  templateUrl: './logs-import.component.html',
  styleUrls: ['./logs-import.component.scss']
})
export class LogsImportComponent implements OnInit {
  files: File[] = [];
  loading: false;

  constructor(private http: HttpClient, private logService: LogsService) {
  }

  ngOnInit(): void {
  }

  onSelect(event): void {
    this.files.push(...event.addedFiles);

    const formData = new FormData();
    // for (let i = 0; i < this.files.length; i++) {
    for (const f of this.files) {
      formData.append('files', f);
    }

    this.logService.importFile(formData).subscribe(res => {
      // Show message for each file uploaded
      for (const m of res) {
        Swal.fire({
          toast: true,
          position: 'top-end',
          icon: m.success ? 'success' : 'warning',
          title: m.message,
          showConfirmButton: false,
          timer: 1500
        });
      }
    });
  }

  onRemove(event): void {
    this.files.splice(this.files.indexOf(event), 1);
  }
}
