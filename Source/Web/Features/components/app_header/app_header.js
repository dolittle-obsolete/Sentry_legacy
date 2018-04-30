/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';

@inject(Router)
export class app_header {
  tenantName = "";
  tenantId = "";

  /**
   * Initializes a new instance of {AppHeader}
   * @param {Router} router 
   */
  constructor(router) {
    this.router = router;
  }


  // https://dolittle.blob.core.windows.net/sentry-tenant-logos/be4c4da6-5ede-405f-a947-8aedad564b7f.png
  attached() {
    this.tenantId = this.router.currentInstruction.params.tenant;

    // Todo: We need the tenant name some how!!
    this.tenantName = this.router.currentInstruction.params.application;
  }
}