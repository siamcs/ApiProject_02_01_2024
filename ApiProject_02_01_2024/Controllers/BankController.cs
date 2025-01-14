using ApiProject_02_01_2024.DTOs;
using ApiProject_02_01_2024.Services.BankService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiProject_02_01_2024.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankController : ControllerBase
    {

        private readonly IBankService _bankService;

        public BankController(IBankService bankService)
        {
            _bankService = bankService;
        }
        

        #region Get
       


        //[HttpGet]
        [HttpGet("{id:int?}")]
        [ProducesResponseType(StatusCodes.Status200OK)] // For successful responses
        [ProducesResponseType(StatusCodes.Status404NotFound)] // If the bank is not found
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] // For exceptions
        public async Task<IActionResult> Index(int ? id)
        {
            try
            {
                
                BankVM bankVM = new BankVM();

                if (id.HasValue && id > 0)
                {
                    bankVM = await _bankService.GetByIdAsync(id.Value);
                    if (bankVM == null)
                    {
                        return NotFound();
                    }
                }

                var bankList = await _bankService.GetAllAsync();

                return Ok(new
                {
                    SelectedBank = bankVM,
                    AllBanks = bankList
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ErrorMessage = ex.Message });
            }
        }

        #endregion


        #region Post
        [HttpPost("Create")]
        public async Task<IActionResult> Create(BankVM bankVM)
        {
            try
            {

                if (string.IsNullOrEmpty(bankVM.BankCode))
                {
                    bankVM.BankCode = await _bankService.GenerateNextBankCodeAsync();
                }


                if (bankVM.Id == 0)
                {
                    var result = await _bankService.SaveAsync(bankVM);
                    if (!result)
                    {
                        return StatusCode(500, "Failed to save the bank.");
                    }
                    return Ok("Saved Successfully");
                }
                else
                {
                    var result = await _bankService.UpdateAsync(bankVM);
                    if (!result)
                    {
                        return StatusCode(500, "Failed to update the bank.");
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
                List<string> errorMessage = new List<string>();
                var result = await _bankService.DeleteAsync(id);
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
                        var result = await _bankService.DeleteAsync(id);
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
        [HttpGet("GenerateNextBankCode")]
        public async Task<IActionResult> GenerateNextBankCode()
        {
            var nextCode = await _bankService.GenerateNextBankCodeAsync();
            return Ok(nextCode);
        }

        #endregion

      


       

    }
}
