import { Component, OnInit } from '@angular/core';
import { VehicleService } from 'src/app/services/vehicle.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { SaveVehicle } from 'src/app/models/vehicle';
import { Vehicle } from './../../models/vehicle';
import * as _ from "underscore";

@Component({
  selector: 'app-vehicle-form',
  templateUrl: './vehicle-form.component.html',
  styleUrls: ['./vehicle-form.component.css']
})
export class VehicleFormComponent implements OnInit {
  makes: any[];
  models: any[];
  features: any[];
  vehicle: SaveVehicle = {
    id: 0,
    makeId: 0,
    modelId: 0,
    isRegistered: false,
    features: [],
    contact: { name: "", email: "", phone: "" }
  };
  constructor(private route: ActivatedRoute, private router: Router, private vehicleService: VehicleService) {
    route.params.subscribe(p => { this.vehicle.id = +p["id"] || 0 });
  }



  private setVehicle(v: Vehicle) {
    this.vehicle.id = v.id;
    this.vehicle.makeId = v.make.id;
    this.vehicle.modelId = v.model.id;
    this.vehicle.isRegistered = v.isRegistered;
    this.vehicle.contact = v.contact;
    this.vehicle.features = _.pluck(v.features, "id");
    // console.log(this.vehicle.features);
  }

  ngOnInit() {
    this.vehicleService.getFeatures().subscribe((features: any[]) => {
      this.features = features;
      this.vehicleService.getMakes().subscribe((makes: any[]) => {
        this.makes = makes;
        if (this.vehicle.id) {
          this.vehicleService.getVehicle(this.vehicle.id).subscribe((v: Vehicle) => {
            this.setVehicle(v);
            this.populateModels();
          });
        }
      });
    });
  }

  onMakeChange() {
    this.populateModels();
    delete this.vehicle.modelId;
  }

  onFeatureToggle(featureId, $event) {
    if ($event.target.checked)
      this.vehicle.features.push(featureId);
    else {
      var index = this.vehicle.features.indexOf(featureId);
      this.vehicle.features.splice(index, 1);
    }
  }
  submit() {
    this.vehicleService.create(this.vehicle)
      .subscribe((v: Vehicle) => this.router.navigate(['/vehicles/', v.id]));
    ;
  }

  private populateModels() {
    var selectedMake = this.makes.find(m => m.id == this.vehicle.makeId);
    this.models = selectedMake ? selectedMake.models : [];
  }

  delete() {
    if (confirm("Are you sure?")) {
      this.vehicleService.delete(this.vehicle.id)
        .subscribe(x => {
          this.router.navigate(["/home"]);
        });
    }
  }

}
