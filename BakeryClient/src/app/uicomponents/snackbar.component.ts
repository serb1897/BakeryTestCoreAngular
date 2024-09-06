import { Component, Input, OnChanges } from '@angular/core';

export interface ISnackBarModel {
    isError: boolean,
    message: string,
    noOffset?: boolean
}

@Component({
    selector: 'snack-bar',
    templateUrl: './snackbar.component.html',
    styleUrls: ['./snackbar.component.css']
})
export class SnackBarComponent implements OnChanges {

    @Input() model: ISnackBarModel;

    showSnackBar: boolean = false;

    ngOnChanges() {
        if (this.model && this.model.message) {
            this.showMessage();
        }
    }

    showMessage() {
        this.showSnackBar = true;
        var e = document.getElementsByClassName("layout");
        e.length > 0 && e[0].scrollIntoView({
            behavior: "smooth",
            block: "end"
        });

        window.setTimeout(() => {
            this.showSnackBar = false;
        }, 5000);
    }

}