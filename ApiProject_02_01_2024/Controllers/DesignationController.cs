﻿
using ApiProject_02_01_2024.DTOs;
using ApiProject_02_01_2024.Services.DesignationService;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject_02_01_2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignationController : ControllerBase
    {
        private readonly IDesignationService _designationService;

        public DesignationController(IDesignationService designationService)
        {
            _designationService = designationService;
          
        }

        #region Get

        [HttpGet("{id:int?}")]
        public async Task<IActionResult> Index(int? id)
        {
            try
            {

                DesignationVM designationVM = new DesignationVM();

                if (id.HasValue && id > 0)
                {
                    designationVM = await _designationService.GetByIdAsync(id.Value);
                    if (designationVM == null)
                    {
                        return NotFound();
                    }
                }

                return Ok(new { data = designationVM });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = ex.Message });
            }
        }




        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var designations = await _designationService.GetAllAsync();

                return Ok(new { AllData = designations });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = ex.Message });
            }
        }

        #endregion


        #region Post
        [HttpPost("Create")]
        public async Task<IActionResult> Create(DesignationVM  designationVM)
        {
            try
            {

                if (string.IsNullOrEmpty(designationVM.DesignationCode))
                {
                    designationVM.DesignationCode = await _designationService.GenerateNextDesignationCodeAsync();
                }


                if (designationVM.DesignationAutoId == 0)
                {
                    var result = await _designationService.SaveAsync(designationVM);
                    if (!result)
                    {
                        return StatusCode(500, "Failed to save.");
                    }
                    return Ok("Saved Successfully");
                }
                else
                {
                    var result = await _designationService.UpdateAsync(designationVM);
                    if (!result)
                    {
                        return StatusCode(500, "Failed to update the.");
                    }
                    return Ok("Updated Successfully");
                }


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        #endregion


        #region Delete
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
               
                var result = await _designationService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound();
                }


                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



        #endregion


        #region Delete multiple

        [HttpPost("DeleteMultiple")]
        public async Task<IActionResult> DeleteMultiple([FromBody] List<int> ids)
        {

            try
            {
                foreach (var id in ids)
                {
                    try
                    {
                        var result = await _designationService.DeleteAsync(id);
                        if (!result)
                        {
                            return NotFound();
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, ex.Message);

                    }
                }

                return Ok("Datum Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion

        #region Next Bank Code 
        [HttpGet("GenerateNextDesignationCode")]
        public async Task<IActionResult> GenerateNextDesignationCodeAsync()
        {
            var nextCode = await _designationService.GenerateNextDesignationCodeAsync();
            return Ok(nextCode);
        }

        #endregion
    }
}
