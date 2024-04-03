import { RouterModule, Routes } from "@angular/router";
import { LogsComponent } from "./components/logs/logs.component";

const appRoutes: Routes = [
    {path: '', component: LogsComponent},
]

export const router = RouterModule.forRoot(appRoutes);